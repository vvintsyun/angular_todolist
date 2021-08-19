import { Component, OnInit } from '@angular/core';
import { LoginService, SecurityService } from '../services/login.service';
import { HttpClient } from '@angular/common/http';
import {Router} from '@angular/router';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent implements OnInit {
  isUserAuthenticated = false;

  constructor(private httpClient: HttpClient,
              private accountService: SecurityService,
              private loginService: LoginService,
              private router: Router) { }

  ngOnInit() {
    this.accountService.isUserAuthenticated$.subscribe(isAuthenticated => {
      this.isUserAuthenticated = isAuthenticated;
      if (this.isUserAuthenticated) {
        this.router.navigate(['/home']);
      }
    });
  }

  login() {
    this.loginService.login();
  }
}
