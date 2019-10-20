let pkey = document.getElementById('pkey');
let lpkey = document.getElementById('pkey');
let dpkey = document.getElementById('dpkey');
let dqrCode = document.getElementById('dqrCode');

const serverUrl = "http://hyper.in:8079/api/Validate/";


window.onload = () => {
  console.log('abriu')
  $.ajax({
    url: serverUrl,
    success: (data) => {
      console.log(data)
      var qrcode = new QRCode('qrcode', {
        text: JSON.stringify(data),
        width: 128,
        height: 128,
        colorDark: "#000000",
        colorLight: "#ffffff",
        correctLevel: QRCode.CorrectLevel.H
      });
      waitingResponse(data.id);
    }
  });

}
function waitingResponse(id) {
  let url = serverUrl + "/" + id;
  let interval = setInterval(() => {
    $.ajax({
      url: url,
      success: (data) => {
        if (data) {
          clearInterval(interval)
          console.log(data)
          windows.close()
          // setCookie("pkey", data, 365)
          // chrome.storage.sync.set({ configuracao: { pkey: data } }, () => { setCookie("pkey", data, 365) });
        } else {
          console.log("sem data")
        }
      }
    })
  }, 2000)
}

chrome.storage.sync.get('configuracao', (data) => {
  if (data.configuracao) {
    dpkey.classList.remove("hidden");
    lpkey.innerText = data.configuracao.pkey;
    dqrCode.classList.add('hidden');
  } else {
    dpkey.classList.add('hidden');
    dqrCode.classList.remove('hidden');
  }
});

var d = new Date();
d.setTime(d.getTime() + (exdays * 24 * 60 * 60 * 1000));
var expires = "expires=" + d.toUTCString();
function setCookie(cname, cvalue, exdays) {

  var d = new Date();
  d.setTime(d.getTime() + (exdays * 24 * 60 * 60 * 1000));
  var expires = "expires=" + d.toUTCString();
  document.cookie = cname + "=" + cvalue + ";" + expires + ";domain=.hyper.in;path=/";

  chrome.cookies.set({ domain: ".hyper.in", name: cname, value: cvalue })
}

function getCookie(cname) {
  var name = cname + "=";
  var ca = document.cookie.split(';');
  for (var i = 0; i < ca.length; i++) {
    var c = ca[i];
    while (c.charAt(0) == ' ') {
      c = c.substring(1);
    }
    if (c.indexOf(name) == 0) {
      return c.substring(name.length, c.length);
    }
  }
  return "";
}










// let btnSalvar = document.getElementById('btnSalvar');
// let dtInicial = document.getElementById('dtInicial');
// let dtFinal = document.getElementById('dtFinal');
// let btDesligar = document.getElementById('btnDesligar');


// btnDesligar.addEventListener('click', () => {
//   btDesligar.value = btnDesligar.value === 'off' ? 'on' : 'off';
//   let disable = btnDesligar.value === 'off';
//   chrome.storage.sync.get('configuracao', (data) => {
//     chrome.storage.sync.set({ configuracao: { dtInicial: data.configuracao.dtInicial, dtFim: data.configuracao.dtFim, disable } }, () => {
//       msg.innerText = (disable ? 'desligado' : 'ligado') + ' com sucesso!';
//     });
//   });

// })
// btnSalvar.addEventListener('click', () => {
//   chrome.storage.sync.set({ configuracao: { dtInicial: dtInicial.value, dtFim: dtFinal.value, disable: btnDesligar.value === 'off' } }, () => {
//     msg.innerText = 'Salvo com sucesso!';
//   });
// })