import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {TaskListComponent} from './task-list/task-list.component';
import { HomeComponent } from './home/home.component';
import {AuthGuard} from './can-activate-guard';

const routes: Routes = [
  { path: '', component: HomeComponent, pathMatch: 'full' },
  { path: 'home', component: TaskListComponent, pathMatch: 'full', canActivate: [AuthGuard] },
  { path: 'tasklist/:id', component: TaskListComponent, canActivate: [AuthGuard] },
  { path: 'tasklistbyurl/:url', component: TaskListComponent },
];

@NgModule({
  imports: [RouterModule, RouterModule.forRoot(routes)],
  exports: [RouterModule],
  providers: [AuthGuard]
})
export class AppRoutingModule {
}
