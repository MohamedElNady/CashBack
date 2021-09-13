// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

//const { swal } = require("../limonte-sweetalert2/sweetalert2");


// Write your JavaScript code.

var routeURL = location.protocol + "//" + location.host;

function onValidateNum() {
	var x = $(first).val() + $(second).val() + $(third).val() + $(fourth).val() + $(fifth).val();
	var y = $(max).val(x);
	var requestData = {
		VerificationCode: x
}

	$.ajax({
		url: routeURL + '/api/AccountSettings/onValidateNum',
		type: 'POST',
		data: JSON.stringify(requestData),
		contentType: "application/json",
		dataType: "json",
		success: function (response) {
			if (response.status === 1) {
				$.notify("Phone Confirmed Successfully", "success");
				setTimeout(function () { window.location.href = 'https://localhost:44346/api/AccountSettings'; }, 1000);
				/*window.location.href = 'https://localhost:44346/api/AccountSettings';*/
			}
			if (response.status === 0) {
				$.notify(response.massage, "error")
				window.location.href = 'https://localhost:44346/api/AccountSettings';
			}

		}
	});

}