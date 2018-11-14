(function(i,s,o,g,r,a,m){i['GoogleAnalyticsObject']=r;i[r]=i[r]||function(){
(i[r].q=i[r].q||[]).push(arguments)},i[r].l=1*new Date();a=s.createElement(o),
m=s.getElementsByTagName(o)[0];a.async=1;a.src=g;m.parentNode.insertBefore(a,m)
})(window,document,'script','//www.google-analytics.com/analytics.js','ga');
ga('create', 'UA-20451773-2');
ga('set', 'allowAdFeatures', false);
ga('set', 'transport', 'beacon');
window.SOLUTION_VERSION = 'VS2017';
ga('send', 'event', 'QuickStart', 'Running', SOLUTION_VERSION);
window.sentMessage = function() {
    ga('send', 'event', 'QuickStart', 'SentMessage', SOLUTION_VERSION);
};

(function ($, window) {
    'use strict';

    var form = $('#license-form'),
        firstname = $('#firstname'),
        lastname = $('#lastname'),
        email = $('#email');

    if (!form.length) {
        console.log('not yet');
        return;
    }

    var d = new Date();
    var trueDate = d.getFullYear() + '-' + (d.getMonth() + 1) + '-' + d.getDate();
    var noop = function () { };
    var version = window.NSB_VERSION;
    var versionOK = version && !!version.match(/^\d+\.\d+\.\d+$/);
    var gaSendEvent = function (category, action, label, callback) {
        console.log(category, action, label);
        var cb = callback || noop;
        if (window.ga) {
            window.ga('send', 'event', category, action, label, { hitCallback: cb });
        } else {
            cb();
        }
    };

    var isEmail = function (email) {
        var regex = /^([a-zA-Z0-9_\.\-\+])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/;
        return regex.test(email);
    };

    ga('send', 'event', 'QuickStart', 'DisplayedLicenseForm', SOLUTION_VERSION);

    $("#submit-license").click(function (e) {

        e.preventDefault();
        var formOK = true;

        var check = function (sel, additionalFn) {
            if (sel.val() === "") {
                sel.attr('placeholder', 'Field is missing');
                formOK = false;
            } else {
                sel.removeAttr('placeholder');
                additionalFn && additionalFn(sel);
            }
        };

        check(firstname);
        check(lastname);
        check(email, function (sel) {
            if (!isEmail(sel.val())) {
                sel.val("");
                sel.attr("placeholder", "Email isn't valid");
                formOK = false;
            } else {
                sel.removeAttr("placeholder");
            }
        });

        if (!formOK) {
            return;
        }

        var postData = {
            FirstName: firstname.val(),
            LastName: lastname.val(),
            Email: email.val(),
            templateid: "FreeLicense"
        };

        if (versionOK) {
            postData.NServiceBusVersion = version;
        }

        ga('send', 'event', 'QuickStart', 'SubmitLicenseForm', SOLUTION_VERSION);

        $.ajax({
            url: "https://api.particular.net/platform/licensetool/requestextension",
            type: "POST",
            data: postData,
            complete: function (xhr, textStatus) {
                if (textStatus === "success") {

                    form.append($('<input/>', { type: 'hidden', id: 'LeadCategory', name: 'LeadCategory', value: 'Download' }))
                        .append($('<input/>', { type: 'hidden', id: 'LeadSource', name: 'LeadSource', value: 'QuickStartTutorial' }))
                        .append($('<input/>', { type: 'hidden', name: 'version', value: 'V7' }));

                    gaSendEvent('Action Performed', 'Trial Extension', 'Day 45', function () {
                        form.submit();
                    });
                }
                else {
                    alert("Unable to send request due to exception. Please contact Particular at support@particular.net");
                }
            }
        });
    });

}(jQuery, window));