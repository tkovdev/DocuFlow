import {AfterContentChecked, AfterViewInit, Component, ViewChild} from '@angular/core';
import { RouterOutlet } from '@angular/router';
import {PdfViewerComponent, PdfViewerModule} from "ng2-pdf-viewer";
import {appConfig} from "./app.config";
import {PdfService} from "./services/pdf.service";
@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, PdfViewerModule],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent implements AfterViewInit{
  title = 'ClientApp';
  dims = {
    w: 215.9,
    h: 279.4,
    xRatio: 0,
    yRatio: 0,
    xTranslate: 0,
    yTranslate: 0
  }

  abortController = new AbortController()

  appliedStamps: {x:number, y: number, node: Node}[] = []

  @ViewChild(PdfViewerComponent) private pdfComponent!: PdfViewerComponent;

  stamper() {
    this.abortController.abort('Start Stamp')
    this.abortController = new AbortController()
    this.pdfComponent.pdfViewerContainer.nativeElement.style.cursor = 'none'

    let tempNode = document.createElement("div")
    tempNode.innerText = '[PLACEHOLDER STAMP]'
    tempNode.style.textAlign = 'center'
    tempNode.style.width = '60mm'
    tempNode.style.height = '15mm'
    tempNode.style.color = 'white'
    tempNode.style.backgroundColor = 'red';
    tempNode.style.opacity = '.5';
    tempNode.style.position = 'fixed'
    tempNode.style.pointerEvents = 'none'
    tempNode.id = `stamp-temp`
    this.pdfComponent.pdfViewer.viewer!.append(tempNode)

    this.pdfComponent.pdfViewerContainer.nativeElement.addEventListener('mousemove', function(ev){
      tempNode.style.top = `${ev.clientY}px`;
      tempNode.style.left = `${ev.clientX}px`;
    }, {signal: this.abortController.signal});

    this.pdfComponent.pdfViewerContainer.nativeElement.addEventListener('click', (event) => {

      this.dims.xRatio = this.dims.w / this.pdfComponent.pdfViewerContainer.nativeElement.clientWidth
      this.dims.yRatio = this.dims.h / this.pdfComponent.pdfViewerContainer.nativeElement.clientHeight
      this.dims.xTranslate = this.dims.xRatio * event.offsetX
      this.dims.yTranslate = this.dims.yRatio * event.offsetY

      let maps = this.appliedStamps.map((v) => {
        return {beginX: v.x, beginY: v.y, endX: v.x + 60, endY: v.y + 15}
      }).filter(v => {
        if (
          (this.dims.xTranslate >= v.beginX && this.dims.xTranslate <= v.endX)
          ||
          (this.dims.xTranslate + 60 >= v.beginX && this.dims.xTranslate + 60 <= v.endX)
        ) {
          if (
            (this.dims.yTranslate >= v.beginY && this.dims.yTranslate <= v.endY)
            ||
            (this.dims.yTranslate + 15 >= v.beginY && this.dims.yTranslate + 15 <= v.endY)
          ) return true
        }

        return false
      });

      if (maps.length <= 0) {
        let node = document.createElement("div")
        node.innerText = '[PLACEHOLDER STAMP]'
        node.style.textAlign = 'center'
        node.style.width = '60mm'
        node.style.height = '15mm'
        node.style.color = 'white'
        node.style.backgroundColor = 'red'
        node.style.position = 'absolute'
        node.style.top = `${this.dims.yTranslate}mm`
        node.style.left = `${this.dims.xTranslate}mm`

        this.pdfComponent.pdfViewer.viewer!.append(node)
        this.appliedStamps.push({x: this.dims.xTranslate, y: this.dims.yTranslate, node})
        this.pdfComponent.pdfViewer.viewer!.removeChild(tempNode)
      }

      this.pdfComponent.pdfViewerContainer.nativeElement.style.cursor = 'unset'
      this.abortController.abort('End Stamp')
      this.abortController = new AbortController()
    }, {signal: this.abortController.signal});
  }

  cancel() {
    this.abortController.abort('Cancelled')
    this.abortController = new AbortController();
    let tempStamp = document.getElementById('stamp-temp')
    if(tempStamp) tempStamp.remove()
  }

  eraser() {
    this.abortController.abort('Start Eraser')
    this.abortController = new AbortController();
    this.pdfComponent.pdfViewerContainer.nativeElement.style.cursor = 'not-allowed'

    this.pdfComponent.pdfViewerContainer.nativeElement.addEventListener('click', (event) => {
      this.dims.xRatio = this.dims.w / this.pdfComponent.pdfViewerContainer.nativeElement.clientWidth
      this.dims.yRatio = this.dims.h / this.pdfComponent.pdfViewerContainer.nativeElement.clientHeight
      this.dims.xTranslate = this.dims.xRatio * event.clientX
      this.dims.yTranslate = this.dims.yRatio * event.clientY

      let maps = this.appliedStamps.map((v) => {
        return {beginX: v.x, beginY: v.y, endX: v.x + 60, endY: v.y + 15, node: v.node}
      }).filter(v => {
        if (
          (this.dims.xTranslate >= v.beginX && this.dims.xTranslate <= v.endX)
        ) {
          if (
            (this.dims.yTranslate >= v.beginY && this.dims.yTranslate <= v.endY)
          ) return true
        }

        return false
      });

      if (maps.length > 0) {
        maps.forEach((v) => {
          let idx = this.appliedStamps.findIndex(v2 => v2.x == v.beginX && v2.y == v.beginY)
          this.appliedStamps.splice(idx, 1)
          this.pdfComponent.pdfViewer.viewer!.removeChild(v.node)
        })
        this.pdfComponent.pdfViewerContainer.nativeElement.style.cursor = 'unset'
        this.abortController.abort('End Eraser')
        this.abortController = new AbortController();
      }

    }, {signal: this.abortController.signal});
  }
  ngAfterViewInit(): void {
  }

  constructor(private pdfService: PdfService) {
  }

  submit() {
    this.pdfService.submitStamps(this.appliedStamps.map(v => { return {x: v.x, y: v.y}})).subscribe((res) => {
      console.log(res)
    });
  }
}
