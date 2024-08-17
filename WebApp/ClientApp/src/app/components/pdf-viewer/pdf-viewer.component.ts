import {Component, OnInit} from '@angular/core';
import {PdfService} from "../../services/pdf.service";
import {Observable} from "rxjs";
import {AsyncPipe, NgIf} from "@angular/common";
import {DomSanitizer} from "@angular/platform-browser";

@Component({
  selector: 'app-pdf-viewer',
  standalone: true,
  imports: [
    NgIf,
    AsyncPipe
  ],
  templateUrl: './pdf-viewer.component.html',
  styleUrl: './pdf-viewer.component.scss'
})
export class PdfViewerComponent implements OnInit {
  pdf$: any;

  constructor(private pdfService: PdfService) {}

  ngOnInit() {
    this.pdf$ = this.pdfService.getPDF();
  }
}
