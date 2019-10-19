chrome.tabs.query({ active: true, currentWindow: true }, function (tabs) {
  chrome.tabs.executeScript(
    tabs[0].id,
    { file: 'teste.js' });
});

function teste() {
  document.body.style.backgroundColor = "#000";
  alert("oi")
}