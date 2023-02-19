import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  public brands?: BrandData[];
  //running phase
  public status: number;


  constructor(private http: HttpClient) {

    this.status = 0; //init
  }

  title = 'BrandCompare';
 

  onDateChange(event: any) {
    this.status = 1; //started
    var date = event.target.value;
    this.getBrands(date);
  }

  getBrands(date: string) {
    this.http.get<BrandData[]>('/brands?date=' + date).subscribe(result => {
      this.brands = result;
      this.status = 2; //ended
    }, error => console.error(error));
  }

}


interface BrandData {
  brandName: string;
  totalProfiles: number;
  totalFans: number;
  totalEngagement: number;
}
