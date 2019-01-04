import {BrowserModule} from '@angular/platform-browser';
import {NgModule} from '@angular/core';
import {FormsModule} from '@angular/forms';
import {HttpClientModule} from '@angular/common/http';

import {AppComponent} from './app.component';
import {NavMenuComponent} from './nav-menu/nav-menu.component';
import {AppRoutingModule} from './app-routing.module';
import {CommonModule} from '@angular/common';
import {TaskListComponent} from './task-list/task-list.component';

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
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
