import {Injectable, SecurityContext} from '@angular/core';
import {HttpClient, HttpHeaders} from "@angular/common/http";
import {map, Observable} from "rxjs";
import {DomSanitizer, SafeResourceUrl} from "@angular/platform-browser";

@Injectable({
  providedIn: 'root'
})
export class PdfService {
  pdfUrl: string = 'https://localhost:7203/api/download'; // Replace with your PDF URL

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
}
