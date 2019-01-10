import {Component} from '@angular/core';
import {TaskDataService, TasklistDataService} from './data.service';
import { LoginService } from './login.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  providers: [TaskDataService, LoginService, TasklistDataService],
  styleUrls: ['./app.component.css']
})
export class AppComponent  {

}
