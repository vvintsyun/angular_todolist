import { Component, OnInit, OnDestroy } from '@angular/core';
import { Subscription } from 'rxjs';
import { SecurityService } from '../login.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent implements OnInit, OnDestroy {

  subscription: Subscription;

  constructor(private accountService: SecurityService) { }

  ngOnInit() {
    this.subscription = this.accountService.isUserAuthenticated.subscribe(isAuthenticated => {
      if (isAuthenticated) {
        //user became authenticated
      } else {
        //user is not authenticated
      }
    });
  }

  ngOnDestroy() {
    this.subscription.unsubscribe();
  }
}
