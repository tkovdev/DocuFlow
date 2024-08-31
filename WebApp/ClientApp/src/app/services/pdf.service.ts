import {Injectable, SecurityContext} from '@angular/core';
import {HttpClient, HttpHeaders} from "@angular/common/http";
import {map, Observable} from "rxjs";
import {DomSanitizer, SafeResourceUrl} from "@angular/platform-browser";

@Injectable({
  providedIn: 'root'
})
export class PdfService {
  pdfUrl: string = 'https://localhost:7148/v1/retrieve/66c066d2276c3a28c846fbec'; // Replace with your PDF URL
  // pdfUrl: string = 'https://localhost:7203/api/download'; // Replace with your PDF URL

  constructor(private http: HttpClient, private sanitizer: DomSanitizer) {}

  getPDF(): Observable<SafeResourceUrl> {
    let headers = new HttpHeaders();
    headers = headers.set('Accept', 'application/octet-stream');

    return this.http.get(this.sanitizer.sanitize(SecurityContext.URL, this.pdfUrl)!, { headers: headers, responseType: 'blob' })
      .pipe(map((val) => this.sanitizer.bypassSecurityTrustResourceUrl(URL.createObjectURL(val))))
  }

  submitStamps(stamps: {x: number, y: number}[]): Observable<boolean> {
    return this.http.post<boolean>('https://localhost:7203/api/stamp', stamps)
  }

  download(): Observable<Blob> {
    //need to get the content type directly from the api
    return this.http.get<any>('https://localhost:7148/v1/retrieve/66c066d2276c3a28c846fbec').pipe(map((res) => {
      return this.b64toBlob(res.data, res.contentType);
    }));
  }

  b64toBlob(b64Data: any, contentType= '', sliceSize= 512) {
    const byteCharacters = atob(b64Data);
    const byteArrays = [];

    for (let offset = 0; offset < byteCharacters.length; offset += sliceSize) {
      const slice = byteCharacters.slice(offset, offset + sliceSize);

      const byteNumbers = new Array(slice.length);
      for (let i = 0; i < slice.length; i++) {
        byteNumbers[i] = slice.charCodeAt(i);
      }

      const byteArray = new Uint8Array(byteNumbers);
      byteArrays.push(byteArray);
    }

    const blob = new Blob(byteArrays, {type: contentType});
    return blob;
  }
}
