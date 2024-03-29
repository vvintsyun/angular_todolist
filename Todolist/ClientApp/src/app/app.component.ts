import {Component, OnInit} from '@angular/core';
import {TaskDataService, TaskListDataService} from './services/data.service';
import { LoginService, SecurityService } from './services/login.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  providers: [TaskDataService, LoginService, TaskListDataService, SecurityService],
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit  {
  public constructor(private accountService: SecurityService) {
  }
  public ngOnInit() {
    this.accountService.updateUserAuthenticationStatus().subscribe();
  }
}
