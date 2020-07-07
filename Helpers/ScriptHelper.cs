using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace SocialNetwork.Helpers
{
    public static class ScriptHelper 
    {
        public static HtmlString GenerarateScript(this HtmlHelper helper, int userID, int interlocutorID)
        {
            TagBuilder builder = new TagBuilder("script");
            string script = @" var request = new XMLHttpRequest()
    function sendMessageRequest() {
        request.open('GET', 'http://192.168.1.68/Chat/ShowNewMessage?userID=userParam&interlocutorID=interlocutorParam')
            request.onreadystatechange = printNewMessage
        request.send()
    }

        function printNewMessage()
        {
            var messageDiv = document.getElementById('messages')
            if (request.readyState == 4)
            {
                var status = request.status
            {
                    if (status == 200)
                    {
                        if (request.responseText.length > 0)
                        {
                            var node = document.createTextNode(request.responseText)
                            messageDiv.appendChild(node)
                        }
                    }
                }
            }
        }

        function startSending()
        {
            setInterval(sendMessageRequest, 1000)
        }

        window.onload = function()
        {
            this.startSending()
        }";
            StringBuilder scriptBuilder = new StringBuilder();
            scriptBuilder.Append(script);
            scriptBuilder.Replace("userParam", userID.ToString());
            scriptBuilder.Replace("interlocutorParam", interlocutorID.ToString());
            builder.InnerHtml = scriptBuilder.ToString();
            return new HtmlString(builder.ToString());
        }
    }
}