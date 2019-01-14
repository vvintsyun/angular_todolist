import {Component, OnInit} from '@angular/core';
import {TaskDataService, TasklistDataService} from './data.service';
import { LoginService, SecurityService, checkIfUserIsAuthenticated } from './login.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  providers: [TaskDataService, LoginService, TasklistDataService, SecurityService],
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit  {
  public constructor(private accountService: SecurityService) {
  }
  public ngOnInit() {    
    this.accountService.updateUserAuthenticationStatus().subscribe(checkIfUserIsAuthenticated(this.accountService));
  }
}
