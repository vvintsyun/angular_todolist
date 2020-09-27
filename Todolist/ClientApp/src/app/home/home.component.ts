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

  constructor(private httpClient: HttpClient, private accountService: SecurityService, private loginService: LoginService) { }

  ngOnInit() {
    this.subscription = this.accountService.isUserAuthenticated.subscribe(isAuthenticated => {
      this.isUserAuthenticated = isAuthenticated;
      if (this.isUserAuthenticated) {
        location.href = '/home';
      }
    });
  }

  login() {
    this.loginService.login();
  }
}
