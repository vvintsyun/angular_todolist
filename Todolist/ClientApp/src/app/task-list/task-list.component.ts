import { Component, OnInit, OnDestroy } from '@angular/core';
import {Task} from '../task';
import {TaskDataService, TasklistDataService} from '../data.service';
import {ActivatedRoute} from '@angular/router';
import { LoginService, SecurityService } from '../login.service';
import { Tasklist } from '../tasklist';
import { PlatformLocation } from '@angular/common';

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
  isEmpty: boolean = true;
  tasklisturl: string;
  readonly urlPath: string;

  constructor(private taskDataService: TaskDataService,
              private taskListDataService: TasklistDataService,
              private route: ActivatedRoute,
              private accountService: SecurityService,
              private location: PlatformLocation) {
    this.urlPath = (location as any).location.origin;
  }

  ngOnInit() {
    this.route.paramMap.subscribe((params) => {
      if (params.get('id')) {
        this.tasklistid = +params.get('id');
        this.loadTasklist(this.tasklistid);
        this.loadTasks(this.tasklistid);
        this.tasklisturl = undefined;
      }
    });
  }

  getUrl() {
    this.taskListDataService.getTasklistUrl(this.tasklistid)
      .subscribe(x => {
        this.tasklisturl = this.urlPath + '/tasklistbyurl/' + x['url'];
      });
  }

  loadTasklist(tasklistId: number) {
    this.taskListDataService.getTasklist(tasklistId)
      .subscribe((data: Tasklist) => {
        this.tasklist = data;
      });
  }

  loadTasks(tasklistid: number) {
    this.taskDataService.getTasks(tasklistid)
      .subscribe((data: Task[]) => {
        this.tasks = data;
        this.isEmpty = this.tasks.length === 0;
      });
  }

  save() {
    if (this.task.id == null) {
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
