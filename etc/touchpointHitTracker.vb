function touchpointHit
	const testing = false 
	'
	dim token
	dim logName,logData
	dim cs
	dim userId,tokenId,isBot,touchUser,sql
	dim returnHtml
	'
	returnHtml = ""
	token = cp.doc.getText("tkn" )
	if token<>"" then
		logData = vbcrlf & now() & vbtab & userId & vbtab & "touchpointHit:" & token
		logData = logData & vbtab & "testing[" & testing & "]"
		set cs = cp.csNew()
		if cs.open( "tracking tokens", "token=" & cp.db.encodeSqlText( token )) then
			tokenId = cs.getInteger( "id" )
		end if
		call cs.close()
		if ( tokenId<>0 ) then
			if instr( 1, cp.request.remoteIp, "173.167.49." )<>0 then
				logData = logData & ",skipping-ContensiveIP[" & cp.request.remoteIp & "]"
			else
				userId=0
				if cs.open( "visitors", "id=" & cp.visitor.id ) then
					userId = cs.getInteger( "memberId" )
				end if
				call cs.close()
				if ( userid <> 0 ) Then
					if not cs.open( "people", "id=" & userId ) Then
						userId=0
					End If
					call cs.close()
				end if
				if userId=0 Then
					userId = cp.user.id
				end if
				logData = logData & ",userid[" & userid & "]"
				touchUser=false
				if testing Then
					sql = "" _
						& "(id=" & userId & ")"
				else
					sql = "" _
						& "(id=" & userId & ")" _
						& "and((admin=0)or(admin is null))" _
						& "and((developer=0)or(developer is null))" _
						& "and((trackingTokenId=0)or(trackingTokenId is null))" _
						& "and((ExcludeFromAnalytics=0)or(ExcludeFromAnalytics is null))"
				end if
				if cs.open( "people", sql ) then
					touchUser=true
				end if
				call cs.close()
				logData = logData & ",touchUser[" & touchUser & "]"
				if touchUser then
					call cp.user.loginById( userId  )
					returnHtml = "<script>jQuery(document).ready(cj.ajax.addon('touchpointAjaxHander','ajaxTkn=" & token & "'));</script>"
				end if
			end if
		end if
		logName = "touchpointTracking\eventLog" & cStr( int( cDbl( now ))) & ".txt"
		call cp.file.appendVirtual( logname, logData )
	end if
	touchpointHit = returnHtml
end function