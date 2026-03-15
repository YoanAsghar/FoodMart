const LOGIN_FORM = document.getElementById("login-form");
const EMAIL = document.getElementById("email");
const PASSWORD = document.getElementById("password");


async function verifyAuthentication(){
  const token = localStorage.getItem("jwt_token");
  if(!token){ return }

  const response = await fetch("http://localhost:5183/api/auth/verify", {
    headers: {"Authorization": `Bearer ${token}`}
  })


  if(!response.ok){
    localStorage.removeItem("jwt_token");
    return
  }

  document.getElementById("login-section").setAttribute("hidden", "true");
  document.getElementById("content").removeAttribute("hidden");
}
function getDataFromLoginForm(){
  return {
    email: EMAIL.value,
    passwordHash: PASSWORD.value
  }
}
verifyAuthentication();

LOGIN_FORM.addEventListener("submit", async (event) => {
  event.preventDefault();

  const UserData = getDataFromLoginForm();
  try{

    const response = await fetch("http://localhost:5183/api/auth/login", {
      method: "POST",
      headers: {
        'Content-Type': 'application/json'
      },
      body: JSON.stringify(UserData)
    })
    if(response.ok){
      const data = await response.json();
      localStorage.setItem("jwt_token", data.token)
      document.getElementById("content").removeAttribute("hidden")
      document.getElementById("login-section").setAttribute("hidden", "true");

      return
    }
  }catch(err){
    return "Error found" + err
  }
})

document.addEventListener("DOMContentLoaded", () => {
  verifyAuthentication()
})
