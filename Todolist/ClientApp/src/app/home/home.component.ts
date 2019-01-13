import { Component, OnInit } from '@angular/core';
import { LoginService, SecurityService } from '../login.service';
import { Subscription } from 'rxjs';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent implements OnInit {
  isUserAuthenticated = false;
  subscription: Subscription;
  userName: string;

  constructor(private httpClient: HttpClient, private accountService: SecurityService, private loginService: LoginService) { }

  ngOnInit() {
    this.subscription = this.accountService.isUserAuthenticated.subscribe(isAuthenticated => {
      this.isUserAuthenticated = isAuthenticated;
      if (this.isUserAuthenticated) {
        this.accountService.getUserName().subscribe(theName => {
          this.userName = theName;
        });
      }
    });
  } 

  login() {
    this.loginService.login();
  }
}
