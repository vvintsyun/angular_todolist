import { Component, OnInit } from '@angular/core';
import {Task} from '../task';
import {TaskDataService, TasklistDataService} from '../data.service';
import {ActivatedRoute} from '@angular/router';
import { LoginService } from '../login.service';
import { Tasklist } from '../tasklist';
import { NavMenuComponent } from '../nav-menu/nav-menu.component';

@Component({
  selector: 'app-task-list',
  templateUrl: './task-list.component.html',
  styleUrls: ['./task-list.component.css']
})
export class TaskListComponent implements OnInit {

  task: Task = new Task();
  tasks: Task[];
  tableMode: boolean = true;
  showTitle: boolean = true;
  tasklist: Tasklist;
  tasklistid: number;
  menuList: NavMenuComponent;
  //tasklistTitle: string;

  constructor(private taskDataService: TaskDataService, private taskListDataService: TasklistDataService, private route: ActivatedRoute, private loginService: LoginService) {
  }

  ngOnInit() {
    this.route.paramMap.subscribe((params) => {
      this.tasklistid = +params.get('id');
      //console.log('Task list id: ' + this.tasklistid);
      this.loadTasklist(this.tasklistid);
      this.loadTasks(this.tasklistid);
    });
  }

  loadTasklist(tasklistid: number) {
    this.taskListDataService.getTasklist(tasklistid)
      .subscribe((data: Tasklist) => this.tasklist = data);
  }

  loadTasks(tasklistid: number) {
    this.taskDataService.getTasks(tasklistid)
      .subscribe((data: Task[]) => this.tasks = data);
  }
  
  save() {
    if (this.task.id == null) {
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
  rename() {
    this.showTitle = false;
  }
  saveTitle() {
    if (this.tasklist.name) {
      this.taskListDataService.updateTasklist(this.tasklist)
        .subscribe(data => this.loadTasklist(this.tasklistid));
      //this.menuList.loadTasklists();
      this.showTitle = true;
    }
  }
  cancelTitle() {
    this.loadTasklist(this.tasklistid);
    this.showTitle = true;
  }
  login() {
    this.loginService.login();
  }

  logout() {
    this.loginService.logout();
  }
}
