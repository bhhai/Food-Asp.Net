
// Validate form
const formLogin = document.getElementById('login__form');
const formRegister = document.getElementById('register__form');
const email = document.getElementById('email');
const password = document.getElementById('password');
const firstName = document.getElementById('first-name');
const lastName = document.getElementById('last-name');
const emailRegis = document.getElementById('regis__email');
const passRegis = document.getElementById('regis__pass');
const loginBtn = document.getElementsByClassName('form__btn')[0];


formLogin.addEventListener('submit', function(e) {
    e.preventDefault()
    
    checkInputs();
});

formRegister.addEventListener('submit', function(e) {
    e.preventDefault()
    
    checkInputs();
});

function checkInputs() {
    const emailValue = email.value.trim();
    const passwordValue = password.value.trim();
    const firstNameVal = firstName.value.trim();
    const lastNameVal = lastName.value.trim();
    const emailRegisVal = emailRegis.value.trim();
    const passwordVal = passRegis.value.trim();

    // validate email
    if(emailValue === '') {
        setErr(email, "Email không được để trống!");
        email.focus();
    } else if(!isEmail(emailValue)) {
        setErr(email, "Địa chỉ email không hợp lệ!")
    }
    else {
        setSuccess(email)
    }

    // validate password
    if(passwordValue === '') {
        setErr(password, "Mật khẩu không được để trống!");
    } else if (passwordValue.length < 6) {
        setErr(password, "Mật khẩu tối thiểu 6 kí tự!")
    } else {
        setSuccess(password);
    }

    // Form Register
    if(firstNameVal === '') {
        setErr(firstName, "Tên không được để trống");
    } else {
        setSuccess(firstName);
    }

    if(lastNameVal === '') {
        setErr(lastName, "Họ không được để trống");
    } else {
        setSuccess(lastName);
    }
    // validate email
    if(emailRegisVal === '') {
        setErr(emailRegis, "Email không được để trống!");
    } else if(!isEmail(emailRegisVal)) {
        setErr(emailRegis, "Địa chỉ email không hợp lệ!")
    }
    else {
        setSuccess(emailRegis)
    }
    // validate password
    if(passwordVal === '') {
        setErr(passRegis, "Mật khẩu không được để trống!");
    } else if (passwordVal.length < 6) {
        setErr(passRegis, "Mật khẩu tối thiểu 6 kí tự!")
    } else {
        setSuccess(passRegis);
    }
}

function setErr(input, message) {
    const formCtl = input.parentElement.parentElement;
    const errMsg = formCtl.querySelector('.form__errMsg');
    input.parentElement.classList.add('form__err');
    errMsg.innerText = message
}

function setSuccess(input) {
    const formCtl = input.parentElement.parentElement;
    const errMsg = formCtl.querySelector('.form__errMsg');
    input.parentElement.classList.remove('form__err');
    errMsg.innerText = null;
}

function isEmail(email) {
	return /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/.test(email);
}


const linkRegis = document.getElementById('link-register');
const linkLogin = document.getElementsByClassName('link__login')[0];

linkRegis.addEventListener('click', function() {
    formRegister.style.display = 'block';
    formLogin.style.display = 'none'
}) 

linkLogin.addEventListener('click', function() {
    formRegister.style.display = 'none';
    formLogin.style.display = 'block'
})
