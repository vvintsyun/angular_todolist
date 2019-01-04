import {Component} from '@angular/core';
import {TaskDataService} from './data.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  providers: [TaskDataService],
  styleUrls: ['./app.component.css']
})
export class AppComponent  {

}
