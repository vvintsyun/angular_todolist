import { Component, OnInit } from '@angular/core';
import {Task} from '../task';
import {TaskDataService} from '../data.service';
import {ActivatedRoute} from '@angular/router';

@Component({
  selector: 'app-task-list',
  templateUrl: './task-list.component.html',
  styleUrls: ['./task-list.component.css']
})
export class TaskListComponent implements OnInit {

  task: Task = new Task();
  tasks: Task[];
  tableMode: boolean = true;
  tasklistid: number;

  constructor(private dataService: TaskDataService, private route: ActivatedRoute) {
  }

  ngOnInit() {
    this.route.paramMap.subscribe((params) => {
      this.tasklistid = +params.get('id');
      console.log('Task list id: ' + this.tasklistid);
      this.loadTasks(this.tasklistid);
    });
  }
  // получаем данные через сервис
  loadTasks(tasklistid: number) {
    this.dataService.getTasks(tasklistid)
      .subscribe((data: Task[]) => this.tasks = data);
  }
  // сохранение данных
  save() {
    if (this.task.id == null) {
      this.dataService.createTask(this.task)
        .subscribe((data: Task) => this.tasks.push(data));
    } else {
      this.dataService.updateTask(this.task)
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
    this.dataService.deleteTask(t.id)
      .subscribe(data => this.loadTasks(this.tasklistid));
  }
  add() {
    this.cancel();
    this.tableMode = false;
  }
}
