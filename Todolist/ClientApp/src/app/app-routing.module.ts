import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {AppComponent} from './app.component';
import {TaskListComponent} from './task-list/task-list.component';

const routes: Routes = [
  { path: '', component: TaskListComponent, pathMatch: 'full'},
  { path: 'tasklist/:id', component: TaskListComponent },
];

@NgModule({
  imports: [RouterModule, RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule {
}
