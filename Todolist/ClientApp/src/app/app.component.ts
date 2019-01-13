import {Component} from '@angular/core';
import {TaskDataService, TasklistDataService} from './data.service';
import { LoginService, SecurityService } from './login.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  providers: [TaskDataService, LoginService, TasklistDataService, SecurityService],
  styleUrls: ['./app.component.css']
})
export class AppComponent  {

}
