
var routeURL = location.protocol + "//" + location.host;

$.ajax({
	url: routeURL + '/api/AccountSettings/GetUserInfo',
	type: 'GET',
	dataType: 'JSON',
	success: function (response) {

		if (response.status === 1) {

			var obj = response.dataeum;

			$(fullname).val(obj.name);
			$(mobile).val(obj.phoneNumber);
			var testmobile = $(mobile).val();
			var z = obj.phoneNumber;
			if (z == null) {
				$(mobile).val(obj.phoneNumber);

			}
			else  {
				var result3 = z.substring(1, 5);
				var result = z.substring(1, 4);
				var result2 = z.substring(1, 3);
				var result4 = z.substring(1, 2);
				var result5 = z.substring(1, 1);
				$('#countriesList').val(result);
				var bla = $('#countriesList').val();

				if (bla == null) {
					$('#countriesList').val(result2);
					var t1 = $('#countriesList').val();
					if (t1 == null) {
						$('#countriesList').val(result3);
						var t2 = $('#countriesList').val();
						if (t2 == null) {
							$('#countriesList').val(result4);
							var t3 = $('#countriesList').val();
							if (t3 == null) {

							}
							else {
								var m1 = testmobile.substring(2);
								$(mobile).val(m1);
							}
						}
						else {
							var m2 = testmobile.substring(5);
							$(mobile).val(m2);
						}
					}
					else {
						var m3 = testmobile.substring(3);
						$(mobile).val(m3);
					}

				}
				else {
					var m4 = testmobile.substring(4);
					$(mobile).val(m4);
				}
				var final = $('#countriesList').val();
			}
			
			$(email).val(obj.email);
			var date = obj.birthdate;
			$(username).val(obj.userName);
			$(currentpass).val(obj.password);
			$(address).val(obj.address);
			if (obj.gender == "Male") {

				$(gender).val('Male')

			}
			if (obj.gender == "Female") {
				$(gender).val('Female')
			}
			if (obj.phoneNumberConfirmed == true) {
				$("#twofactorval").hide();
			}
			if (obj.phoneNumberConfirmed == false) {
				$("#ptext").hide();
			}
		
			//var parsed_date = time_value.replace(/\//g, '-');
			$(appointmentDate).val(date);

		}


	},
	async: false
});

function onUpdateButton() {
	var bug = $(mobile).val()
	if (bug == null || bug=="") {
		var requestData = {

			Name: $(fullname).val(),
			PhoneNumber: $(mobile).val(),
			Birthdate: $(appointmentDate).val(),
			userName: $(username).val(),
			Address: $(address).val(),
			Gender: $(gender).val()

		};
		$("#countriesList").val("choose");
	
	}

	else {
		var x = $('#countriesList').val();
		var y = "+" + x + + $(mobile).val();
		var z = y;
		var requestData = {

			Name: $(fullname).val(),
			PhoneNumber: z,
			Birthdate: $(appointmentDate).val(),
			userName: $(username).val(),
			Address: $(address).val(),
			Gender: $(gender).val()

		};

		var result3 = z.substring(1, 5);
		var result = z.substring(1, 4);
		var result2 = z.substring(1, 3);
		var result4 = z.substring(1, 2);
		var result5 = z.substring(1, 1);
		$('#countriesList').val(result);
		var bla = $('#countriesList').val();

		if (bla == null) {
			$('#countriesList').val(result2);
			var t1 = $('#countriesList').val();
			if (t1 == null) {
				$('#countriesList').val(result3);
				var t2 = $('#countriesList').val();
				if (t2 == null) {
					$('#countriesList').val(result4);
					var t3 = $('#countriesList').val();
					if (t3 == null) {
						$('#countriesList').val(result5);
						var t4 = $('#countriesList').val();

					}
				}
			}

		}
		var final = $('#countriesList').val();
	}
	

	
	$.ajax({
		url: routeURL + '/api/AccountSettings/UpdateUserInfo',
		type: 'POST',
		data: JSON.stringify(requestData),
		contentType: "application/json",
		dataType: "json",
		success: function (response) {

			if (response.status === 1) {

				
				
				$.notify("Data Changed Successfully", "Sucess")
				setTimeout(function () { location.reload() }, 1000);
			}

		/*	location.reload(true);*/
		}

	});
}
//}
function ondeleteAccount() {

	new swal({
		title: 'Are you sure?',
		text: "You Account will be Deleted!",
		icon: 'warning',
		showCancelButton: true,
		confirmButtonColor: '#3085d6',
		cancelButtonColor: '#d33',
		confirmButtonText: 'Yes, delete it!'
	}).then((result) => {
		if (result.isConfirmed) {


			$.ajax({

				url: routeURL + '/api/AccountSettings/DeleteUser',
				type: 'POST',
				success: function (response) {
					if (response.status === 1) {

						new swal('Deleted!',
							'Your Account has been deleted.',
							'success').then(function () {
								window.location.href = 'https://localhost:44346/Home/Index';
							});

					}


					if (response.status === 0) {

						new swal('Failed', 'Account not deleted please contact support', 'warning');

					}
				}
			});


		}
	});

}

function ChangeCurrentPassword() {
	if (checkValidation()) {
		var requestData = {

			Password: $(currentpass).val(),
			NewPassword: $(newpass).val(),
			ConfirmPassword: $(confirmpass).val()


		};
		if (document.getElementById('confirmpass').value ==
			document.getElementById('newpass').value) {
			new swal({
				title: 'Are you sure?',
				text: "Your Password Will Be Changed!",
				icon: 'info',
				showCancelButton: true,
				confirmButtonColor: '#3085d6',
				cancelButtonColor: '#d33',
				confirmButtonText: 'Yes, change it!'
			}).then((result) => {
				if (result.isConfirmed) {


					$.ajax({

						url: routeURL + '/api/AccountSettings/ChangeCurrentPassword',
						type: 'POST',
						data: JSON.stringify(requestData),
						contentType: "application/json",
						dataType: "json",
						success: function (response) {
							if (response.status === 1) {

								new swal('Changed!',
									'Your Password has been changed.',
									'success')


								$(currentpass).val("")
								$(confirmpass).val("")
								$(newpass).val("")


							}
							if (response.status === 0) {

								new swal('Failed', 'Password not changed please contact support', 'warning');

							}

						},
						error: function (xhr) {
							$.notify("Error", "error");
						}
					});
				}
			});
		}

		else if (document.getElementById('confirmpass').value !=
			document.getElementById('newpass').value) {

			$.notify("Password and confirm password aren't match", "fail")

		}
	}
	else {

		$.notify("Fill the Lines", "fail")
	}
}

function checkValidation() {

	var isValid = false;
	var pass = document.getElementById("currentpass").value;
	var newpass = document.getElementById("newpass").value;
	var confirmpass = document.getElementById("confirmpass").value;
	if (pass == "" || pass === undefined) {
		$("#currentpass").css("border", "1px solid red");
		$("#currentpass").focus();
		var isValid = false;
	}
	else {
		$("#currentpass").css("border", "1px solid #ced4da");
		isValid = "true"
	}
	if (newpass == "" || newpass === undefined) {
		$("#newpass").css("border", "1px solid red");
		$("#newpass").focus();
		var isValid = false;
	}
	else {
		$("#newpass").css("border", "1px solid #ced4da");
		isValid = "true"
	}
	if (confirmpass == "" || confirmpass === undefined) {
		$("#confirmpass").css("border", "1px solid red");
		$("#confirmpass").focus();
		var isValid = false;
	}
	else {
		$("#confirmpass").css("border", "1px solid #ced4da");
		isValid = "true"
	}

	return isValid;

}

function VerifyPhoneNumber() {
	var requestData = {
		PhoneNumber: $(mobile).val()
	}

	$.ajax({
		url: routeURL + '/api/AccountSettings/VerifyPhoneNumber',
		type: 'POST',
		data: JSON.stringify(requestData),
		contentType: "application/json",
		dataType: "json",
		success: function (response) {
			if (response.status===1) {
				window.location.href = "https://localhost:44346/api/AccountSettings/ConfirmPhone2";
			}
			
		}
	});
}
