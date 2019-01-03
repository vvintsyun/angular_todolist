import { Component, OnInit } from '@angular/core';
import { TaskDataService } from './data.service';
import { Task } from './task';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  providers: [TaskDataService],
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {

  task: Task = new Task();
  tasks: Task[];
  tableMode: boolean = true;
  tasklistid: number;

  constructor(private dataService: TaskDataService, private route: ActivatedRoute) { }

  ngOnInit() {        
    this.route.queryParams.subscribe(params => {
      this.loadTasks(params['tasklistid']);
      this.tasklistid = params['tasklistid'];
    });    
  }
  // получаем данные через сервис
  loadTasks(tasklistid: number) {
    console.log(tasklistid);
    this.dataService.getTasks()
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
