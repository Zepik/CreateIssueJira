$(function() {
       $('#files').bind('change', function() {
            filesSize = 0;
            for (var i = 0; i < this.files.length; i++) 
            {  
                filesSize += this.files[i].size;
                if(filesSize > 4194304){
                    document.getElementById("Message").innerHTML = "Maksymalny rozmiar plików to <b>4MB</b>";
                    $('#submit').prop('disabled', true);
                }
                else
                {
                    $('#submit').prop('disabled', false);
                    document.getElementById("Message").innerHTML = "";
                }
            }
        });

          $(document).ready(function() {
            filesSize = 0;
            var files_element = document.getElementById("files")
            for (var i = 0; i < files_element.files.length; i++) 
            {  
                filesSize += files_element.files[i].size;
                if(filesSize > 4194304){
                    document.getElementById("Message").innerHTML = "Maksymalny rozmiar plików to <b>4MB</b>";
                    $('#submit').prop('disabled', true);
                }
                else
                {
                    $('#submit').prop('disabled', false);
                    document.getElementById("Message").innerHTML = "";
                }
            }
        });

}) 