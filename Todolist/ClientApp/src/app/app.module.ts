import {BrowserModule} from '@angular/platform-browser';
import {NgModule, APP_INITIALIZER} from '@angular/core';
import {FormsModule} from '@angular/forms';
import {HttpClientModule, HTTP_INTERCEPTORS} from '@angular/common/http';

import {AppComponent} from './app.component';
import {UserListsComponent} from './user-lists/user-lists.component';
import {AppRoutingModule} from './app-routing.module';
import {CommonModule} from '@angular/common';
import {TaskListComponent} from './task-list/task-list.component';
import { HomeComponent } from './home/home.component';
import { TaskListUrlComponent } from './task-list/task-list-url.component';

@NgModule({
  declarations: [
    AppComponent,
    UserListsComponent,
    TaskListComponent,
    HomeComponent,
    TaskListUrlComponent
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    CommonModule,
    FormsModule,
    AppRoutingModule
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
