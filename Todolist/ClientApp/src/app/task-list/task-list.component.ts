import { Component, OnInit, OnDestroy } from '@angular/core';
import {Task} from '../task';
import {TaskDataService, TasklistDataService} from '../data.service';
import {ActivatedRoute} from '@angular/router';
import { LoginService, SecurityService } from '../login.service';
import { Tasklist } from '../tasklist';
import { NavMenuComponent } from '../nav-menu/nav-menu.component';
import { Subscription } from 'rxjs';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-task-list',
  templateUrl: './task-list.component.html',
  styleUrls: ['./task-list.component.css']
})
export class TaskListComponent implements OnInit {  

  task: Task = new Task();
  tasks: Task[];
  tableMode: boolean = true;
  tasklist: Tasklist;
  tasklistid: number;
  showHeader: boolean;
  tasklisturl: string;

  constructor(private taskDataService: TaskDataService, private taskListDataService: TasklistDataService, private route: ActivatedRoute, private accountService: SecurityService) {
  }

  ngOnInit() {    
    this.route.paramMap.subscribe((params) => {
      this.tasklistid = +params.get('id');
      //console.log('Task list id: ' + this.tasklistid);
      this.loadTasklist(this.tasklistid);
      this.loadTasks(this.tasklistid);
    });
  }

  getUrl() {
    this.taskListDataService.getTasklistUrl(this.tasklistid).subscribe(url => this.tasklisturl = url);
  }

  checkUserAccess() {
    this.accountService.getUserId().subscribe(userid => {
      if (this.tasklist.user != userid /*URL_CHECK*/) location.href = '/home';
    });
  }
  
  loadTasklist(tasklistid: number) {
    this.taskListDataService.getTasklist(tasklistid)
      .subscribe((data: Tasklist) => { this.tasklist = data; if (typeof data !== 'undefined') this.checkUserAccess(); });
  }

  loadTasks(tasklistid: number) {
    this.taskDataService.getTasks(tasklistid)
      .subscribe((data: Task[]) => { this.tasks = data; this.showHeader = this.tasks.length > 0; });
  }
  
  save() {
    if (this.task.id == null) {
      //this.task.tasklist = this.tasklist;      
      this.task.tasklistid = this.tasklist.id;      
      this.taskDataService.createTask(this.task)
        .subscribe((data: Task) => this.tasks.push(data));
    } else {
      this.taskDataService.updateTask(this.task)
        .subscribe(data => this.loadTasks(this.tasklistid));
    }
    this.cancel();
  }
  editTask(t: Task) {
    this.task = t;
  }
  cancel() {
    this.task = new Task();
    this.tableMode = true;
  }
  delete(t: Task) {
    this.taskDataService.deleteTask(t.id)
      .subscribe(data => this.loadTasks(this.tasklistid));
  }
  add() {
    this.cancel();
    this.tableMode = false;
  }  
}
