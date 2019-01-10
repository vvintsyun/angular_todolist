import {BrowserModule} from '@angular/platform-browser';
import {NgModule, APP_INITIALIZER} from '@angular/core';
import {FormsModule} from '@angular/forms';
import {HttpClientModule, HTTP_INTERCEPTORS} from '@angular/common/http';

import {AppComponent} from './app.component';
import {NavMenuComponent} from './nav-menu/nav-menu.component';
import {AppRoutingModule} from './app-routing.module';
import {CommonModule} from '@angular/common';
import {TaskListComponent} from './task-list/task-list.component';
//import { Interceptor401Service } from './login.service';
import { checkIfUserIsAuthenticated, SecurityService } from './login.service';

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    TaskListComponent,
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    CommonModule,
    FormsModule,
    AppRoutingModule
  ],
  providers: [{ provide: APP_INITIALIZER, useFactory: checkIfUserIsAuthenticated, multi: true, deps: [SecurityService] }/*{ provide: HTTP_INTERCEPTORS, useClass: Interceptor401Service, multi: true }*/],
  bootstrap: [AppComponent]
})
export class AppModule { }
