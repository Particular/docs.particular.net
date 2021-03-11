'use strict';
(function () {
    var now = new Date();
    var x = document.getElementsByTagName("tr");
    var i;
    for (i = 0; i < x.length; i++) {
        var tr = x[i];
    
        if(tr.childNodes.length>3)
        {
            var dateString = tr.childNodes[2].innerText
            var extended = tr.childNodes[3].innerText
            var isExtended = extended.startsWith("Extended");
            if(isExtended)
            {
                dateString = extended.slice(-10);
            }
            var d = new Date(dateString);
            var isPast = now > d;
            if(isPast)
            {
                tr.style='text-decoration:line-through';
            }
        }
    }
} ());