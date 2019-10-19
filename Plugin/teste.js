flag = false;
flag1 = false;
flag2 = false;
flag3 = false;

let dtInicial, dtFinal;

chrome.storage.sync.get('configuracao', (data) => {
  dtInicial = formatDate(data.configuracao.dtInicial);
  dtFinal = formatDate(data.configuracao.dtFim);
  console.log(data.configuracao.disable)
  if (data.configuracao.disable) {
    return;
  }
  observar("etapa7", () => {
    if ($("#etapa7").css('display') === 'block') {
      if ($(".has-sub>ul").length > 0) {
        console.log('etapa7')
        $(".has-sub>ul").css("display", 'block');
        //window.scrollTo(0, document.body.scrollHeight);
        // $('html,body').animate({ scrollTop: document.body.scrollHeight }, "slow");
        // clearInterval(timer);

      }
    }

  });
  // observar("janela1", () => {
  //   if ($("#janela1").css('display') === 'block')
  //     escondeMsg();
  // })
  observar("janela1Quarto", () => {
    $("#ImgSelecionarQuarto").click();
  })
  observar("etapa3", () => {
    $("#hdnMesIDAnterior").val(getMes(dtInicial));
    $("#hdnStrDe").val(dtInicial)
    $("#hdnStrAte").val(dtFinal)
  })
  observar("lyCalendar", () => {
    console.log('etapa3 >  calendario');

    $("#hdnMesIDAnterior").val(getMes(dtInicial));
    $("#hdnStrDe").val(dtInicial)
    $("#hdnStrAte").val(dtFinal)

    if ($("#lyCalendar").css("display") === 'block') {
      console.log("calendario")
      // setPeriodo('03/12/2018', 94);
      // setPeriodo('07/12/2018', 97);

    }
  })
  observar("etapa4", () => {
    if ($("#etapa4").css('display') === 'block') {
      SomarHospedes(1);
      SomarHospedes(1);
      SomarHospedes(3);
      console.log('etapa4')
    }
  })
  valorSugerido = false
  observar("etapa5", () => {
    if ($("#etapa5").css('display') === 'block') {
      OutrasOpcoes()
      if (valorSugerido)
        $("#LyValorSug").click();
      else
        IniciarReserva(0, 25, 0, 0, 25, 0)
    }
  })
  observar("etapa6", () => {
    if ($("#etapa6").css('display') === 'block') {
      login();
    }
  })
  observar("etapa8", () => {
    if ($("#etapa8").css('display') === 'block') {
      $("#Hos0").click()
      $("#Hos1").click()
      console.log('etapa8')
      mostraTotal();
    }
  })

  observar("etapa9", () => {
    if ($("#etapa9").css('display') === 'block') {
      $("#btnTermo").click();
      console.log('etapa9')
    }
  })
  observar("hdnAtivarLoginInicio", () => {
    if ($("#hdnAtivarLoginInicio").val() == "1")
      alert('oi');
  })
  observar("hdnlogado", () => {
    if ($("#hdnlogado").val() == '0') {
      login()
    }
  })

  observar("Clock", () => {
    if ($("#Clock").css("display") === "none") {
      login()


      // timer = setInterval(() => {

      //   if ($("#hdnAtivarLoginInicio").val() == "1" && $("#hdnlogado").val() == '0' && $("#Clock").css("display") === "none" && !flag) {
      //     flag = true;
      //     console.log('login')
      //     let form = document.forms[0]
      //     form.txtMatricula.value = '35445672883';
      //     form.txtPwd.value = 'voyage';
      //     identificacao(() => {
      //       console.log('logado')
      //       $("#etapa1").css('display', 'block')

      //       $("#hdnMesIDAnterior").val(getMes(dtInicial));
      //       $("#hdnStrDe").val(dtInicial)
      //       $("#hdnStrAte").val(dtFinal)
      //       $("#hdnlogado").val(1)
      //       flag = false;
      //     });
      //   }
      //   else if ($("#hdnAtivarLoginInicio").val() == "0" && $("#hdnlogado").val() == '0' && !flag) {
      //     flag = true;
      //     console.log('login')
      //     let form = document.forms[0]
      //     form.txtMatricula.value = '35445672883';
      //     form.txtPwd.value = 'voyage';
      //     identificacao(() => {
      //       console.log('logado')
      //       $("#etapa1").css('display', 'block')

      //       $("#hdnMesIDAnterior").val(getMes(dtInicial));
      //       $("#hdnStrDe").val(dtInicial)
      //       $("#hdnStrAte").val(dtFinal)
      //       $("#hdnlogado").val(1)
      //       flag = false;
      //     });
      //   }

      // }, 100);
    }
  })
  login()
})

function login() {
  console.log('login')
  let form = document.forms[0]
  form.txtMatricula.value = '35445672883';
  form.txtPwd.value = 'voyage';
  identificacao(() => {
    console.log('logado')
    $("#etapa1").css('display', 'block')

    $("#hdnMesIDAnterior").val(getMes(dtInicial));
    $("#hdnStrDe").val(dtInicial)
    $("#hdnStrAte").val(dtFinal)
    $("#hdnlogado").val(1)
    flag = false;
  });


}


function observar(id, evento) {
  var targetNode = document.getElementById(id);
  var observer = new MutationObserver(evento);
  observer.observe(targetNode, { attributes: true, childList: false });
}

//document.body.style.backgroundColor = "#000";
//$("body").css('background-color', "#fff");
function getMes(text) {
  let data = text.split('-');
  var mes = data[1];
  if (mes && mes.toString().length == 1)
    mes = "0" + mes;

  return mes;
}
function formatDate(text) {
  if (!text)
    return '';
  let data = text.split('-');
  var dia = data[2];
  if (dia.toString().length == 1)
    dia = "0" + dia;
  var mes = data[1];
  if (mes.toString().length == 1)
    mes = "0" + mes;
  var ano = data[0];
  return dia + "/" + mes + "/" + ano;
}