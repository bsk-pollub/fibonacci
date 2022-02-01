import { HttpClient } from '@angular/common/http';
import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-fibcalc',
  templateUrl: './fibcalc.component.html',
  styleUrls: ['./fibcalc.component.css']
})
export class FibcalcComponent implements OnInit {

  constructor(private http: HttpClient) { }

  public n: number = 0;

  public currFib: number = 0;

  public history: any = null;

  ngOnInit(): void {
  }

  calcFib(): void {
    this.http.post<any>('http://localhost/fibonacci', { n: this.n }).subscribe(data => {
      if (data.success) {
        this.currFib = data.value;
      }
      else {
        alert(data.error);
      }
    });
  }

  getHistory(): void {
    this.http.get<any>('http://localhost/history').subscribe(data => {
      this.history = data;
    });
  }

}
