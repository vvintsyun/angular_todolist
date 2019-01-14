import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {AppComponent} from './app.component';
import {TaskListComponent} from './task-list/task-list.component';
import { HomeComponent } from './home/home.component';
import { TaskListUrlComponent } from './task-list/task-list-url.component';

const routes: Routes = [
  { path: '', component: HomeComponent, pathMatch: 'full' },
  { path: 'home', component: TaskListComponent, pathMatch: 'full' },
  { path: 'tasklist/:id', component: TaskListComponent },
  { path: 'tasklistbyurl/:url', component: TaskListUrlComponent },
];

@NgModule({
  imports: [RouterModule, RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule {
}
