'
' tochpointHitTracker - gets tkn, qualifies hit and adds touchpointAjax with ajaxTkn
' tochpointAjax - javascript to hit touchpointAjaxHander onReady with ajaxTkn
' touchpointAjaxHandler - gets ajaxTkn and sets user token and group
'
function touchpointAjaxHandler
	dim token
	dim logName,logData
	dim cs
	dim userId,tokenId,isBot,touchUser,sql
	token = cp.doc.getText("ajaxTkn" )
	if token<>"" then
		set cs = cp.csNew()
		if cs.open( "tracking tokens", "token=" & cp.db.encodeSqlText( token )) then
			tokenId = cs.getInteger( "id" )
		end if
		call cs.close()
		if ( tokenId<>0 ) then
			logData = vbcrlf & now() & vbtab & userId & vbtab & "touchpointAjaxHandler:" & token
			if true Then
				userId = cp.user.id
				if true then
					touchUser=false
					sql = "" _
						& "(id=" & userId & ")" _
						& "and((admin=0)or(admin is null))" _
						& "and((developer=0)or(developer is null))" _
						& "and((trackingTokenId=0)or(trackingTokenId is null))" _
						& "and((ExcludeFromAnalytics=0)or(ExcludeFromAnalytics is null))"
					if cs.open( "people", sql ) then
						logData = logData & vbtab & "nonAdmin/nonDev/notTracked/notExclude+setUserToken"
						touchUser=true
						call cs.setField( "trackingTokenId", tokenId )
						if ( cs.getText("username")="" ) then
							logData = logData & "+setUsername"
							call cs.setField( "username", "guest" & userId )
						end if
					end if
					call cs.close()
					if touchUser then
						logData = logData & "+addedToTrafficGroup"
						call cp.group.addUser( "Inbound Marketing Traffic" )
						logName = "touchpointTracking\eventLog" & cStr( int( cDbl( now ))) & ".txt"
						call cp.file.appendVirtual( logname, logData )
					end if
				end if
			end if
		end if
	end if
	m = ""
end function