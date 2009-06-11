        var outputPanel;
    
        function getPostContent(theForm, outputTo) 
        { 
            var request = new Sys.Net.WebRequest(); 
            outputPanel=outputTo;
            request.set_httpVerb("POST");
            request.set_url(theForm.action);
            
            var body="";
            for(i=0; i<theForm.elements.length; i++){
                formValue=String.format("{0}={1}&", theForm.elements[i].name,theForm.elements[i].value);
                body +=formValue;
            }
            body+="__ajax=true";
            request.set_body(body);
            request.get_headers()["Content-Length"] = body.length;

            request.add_completed(updatePage);
            request.invoke(); 
        }   
        function getContent(url, outputTo) 
        { 
            var request = new Sys.Net.WebRequest(); 
            outputPanel=outputTo;
            request.set_url(url);
            request.set_httpVerb("GET"); 
            request.add_completed(updatePage);
            request.invoke(); 
        } 

        function updatePage(executor, eventArgs)
        {  
            if(executor.get_responseAvailable())
            { 
                $get(outputPanel).innerHTML = executor.get_responseData();
            } 
            else
            {
            if (executor.get_timedOut()) 
                alert("Timed Out"); 
            else if (executor.get_aborted()) 
                alert("Aborted"); 
            }  
        }
       
        function success(results){
            $get("Results").innerHTML = results;

        }
        function fail(err){
            alert(err);
        }