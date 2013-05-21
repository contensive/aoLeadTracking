function m
  dim block
  dim button
  dim link,name,token
  dim cs
  set block = cp.blockNew()
  button = cp.doc.getText( "button" )
  if button<>"" then
    name = cp.doc.getText( "trackingName" )
    token = cp.utils.createGuid()
    token = replace( token, "}", "" )
    token = replace( token, "{", "" )
    token = replace( token, "-", "" )
    link = cp.doc.getText( "trackingUrl" )
    link = replace( link, "&RequestBinary=True", "", 1 )
    if instr( 1, link, "?" )=0 then
      link = link & "?tkn=" & token
    else
      link = link & "&tkn=" & token
    end if
    set cs = cp.csNew()
    if cs.insert( "Lead Sources" ) then
      call cs.setField( "name", name )
      call cs.setField( "link", link )
      call cs.setField( "token", token )
    end if
    m = m & cp.html.div( "<br><br>use this URL" )
    m = m & cp.html.div( "<input class=""output"" value=""" & link & """>" )
  end if
  call block.openLayout( "Lead Source Generator" )
  m = cp.html.form( block.getHtml()) & m
  m = cp.html.div( m,"","ttCon" )
end function