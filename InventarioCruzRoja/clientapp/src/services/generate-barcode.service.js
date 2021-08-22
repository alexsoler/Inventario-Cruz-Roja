import { jsPDF as JsPDF } from 'jspdf'
import JsBarcode from 'jsbarcode'

const generateAndDownloadBarcodeInPDF = (orderNo, count, configJsBarcode) => {
  const makeBase64Image = convertTextToBase64Barcode(orderNo, configJsBarcode)

  convertBase64ToPNGImage(makeBase64Image).then((realImage) => {
    const doc = new JsPDF('p', 'mm', 'a4')

    for (let index = 0, y = 10; index < count; index++, y += 50) {
      if (index > 0 && index % 5 === 0) {
        doc.addPage()
        y = 10
      }

      doc.addImage(realImage, 'PNG', 10, y)
    }

    const string = doc.output('datauristring')
    var embed = "<embed width='100%' height='100%' src='" + string + "'/>"
    var x = window.open()
    x.document.open()
    x.document.write(embed)
    x.document.close()
  })
}

const convertBase64ToPNGImage = (url) => {
  return new Promise((resolve) => {
    const img = new Image()
    img.onload = () => resolve(img)
    img.src = url
  })
}

const convertTextToBase64Barcode = (text, options) => {
  const canvas = document.createElement('canvas')
  JsBarcode(canvas, text, options)
  return canvas.toDataURL('image/png')
}

export { generateAndDownloadBarcodeInPDF }
