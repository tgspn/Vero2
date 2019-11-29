let veroElement = document.getElementById('vero');
let btn = document.createElement('button');
let storeName = document.querySelectorAll('[data-store-name]')[0];
let value = document.querySelectorAll('[data-value]')[0];
let fields = []
btn.textContent = "Utilizar vero";
veroElement.appendChild(btn)
document.querySelectorAll('[data-field]').forEach((e) => fields.push(e.getAttribute('data-field')))
btn.addEventListener('click', () => {
  $.ajax({
    url: "http://hyper.in:8079/api/info",
    method: "Post",
    contentType: "application/json",
    crossDomain: true,
    xhrFields: {
      withCredentials: true
    },
    data: JSON.stringify({
      //Id: "355db715-e036-4ccf-8420-9395d5d21159",
      StoreName: storeName.value,
      Fields: fields,//["nome", "endereco", "email"],
      Value: value.value ? value.value : value.innerText
    }
    ),
    success: (d, Status, jqXHR) => {
      if (jqXHR.status == 200) {
        waitResult()
      } else {
        alert("O servidor retornou um erro");
      }
    },
    error: (e) => {
      console.log(e)
    }

  });
});


function waitResult() {
  let timer = setInterval(() => {
    $.ajax({
      url: "http://hyper.in:8079/api/info/",
      method: "Get",
      contentType: "application/json",
      crossDomain: true,
      xhrFields: {
        withCredentials: true
      },
      success: (d, Status, jqXHR) => {
        if (jqXHR.status == 200) {
          clearInterval(timer)
          for (let key in d) {
            let element = document.querySelectorAll(`[data-field='${key}']`)[0];
            console.log(key, d[key], element)
            element.value = d[key];
          }

          // $("data").val(d["nome"])
          // $("#endereco").val(d["endereco"])

        } else if (jqXHR.status == 406) {
          clearInterval(timer)
          alert("As informações não foram autorizadas a ser compartilhadas");

        }
      },
      error: (e) => {
        console.log(e)
        clearInterval(timer)
      }
    });
  }, 1000);
}