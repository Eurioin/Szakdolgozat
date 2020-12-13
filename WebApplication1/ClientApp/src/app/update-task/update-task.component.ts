import { Component, OnInit } from '@angular/core';
import { FormControl } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { AuthorizeService } from 'src/api-authorization/authorize.service';
import { FetcherService } from '../fetcher.service';
import { Project } from '../models/project';
import { SubTask } from '../models/sub-task';
import { Task } from '../models/task';

@Component({
  selector: 'app-update-task',
  templateUrl: './update-task.component.html',
  styleUrls: ['./update-task.component.css']
})
export class UpdateTaskComponent implements OnInit {

  public Project: Project = new Project();
  public Users: Array<string> = [];
  public Selected: Array<boolean> = [];
  public id: string = "";
  public name: string = "";
  public subtasks: string = "";
  public description: string = "";
  public type: string = "";
  public status: string = "";
  public priority: number;
  public date: Date = new Date("1/1/1998");
  public project: string = "";

  constructor(private fetcher: FetcherService, private authorizeService: AuthorizeService, private router: Router, private route: ActivatedRoute) { 
  }

  ngOnInit() {
    this.authorizeService.isAuthenticated().subscribe(auth => {
      if (!auth) {
        this.router.navigate(["/authentication/login"]);
      } else {
        this.getTask();
      }
    });
  }

  getTask() {
    this.id = this.route.snapshot.paramMap.get('id');
    this.fetcher.getTaskFromApi(this.id).subscribe(resp => {
      this.name = resp.name;
      this.description = resp.description;
      this.status = resp.status;
      this.type = resp.type;
      this.date = resp.endDate;
      this.priority = resp.priority;
      this.project = resp.project;
      this.fetcher.getProjectFromApi(this.project).subscribe(r => {
        if (r !== null) {
          this.Project = r;
          this.Selected = [];
          this.Project.assignees.forEach(a => {
            this.fetcher.getAccountFromApiById(a).subscribe(r => {
              if (!this.Users.includes(r.username)) {
                this.Users.push(r.username);
              }

              if (resp.users.includes(r.username + ' - ' + r.email)) {
                this.Selected.push(true);
              } else {
                this.Selected.push(false);
              }
            }, err => console.log(err));
          });
        }
      }, err => console.log(err));
    });
    (<HTMLInputElement>document.getElementById('desc')).value = this.description;
    (<HTMLInputElement>document.getElementById('date')).value = this.date.toString();

  }

  edit() {
    var t = new Task();
    t.id = this.id;
    t.name = this.name;
    t.description = this.description;
    t.endDate = this.date;
    t.priority = this.priority;
    t.type =this.type;
    t.status =this.status;
    t.subTasks = [];
    this.subtasks.split(';').forEach(sb => {
      var s = new SubTask();
      s.description = sb;
      t.subTasks.push(s);
    });
    t.users = [];
    this.Users.forEach((user, idx) => {
      if (this.Selected[idx]) {
        t.users.push(user);
      }
    });
    t.project = this.project;
    this.fetcher.postUpdateTaskToApi(t).subscribe(resp => this.router.navigate(["project", this.project]), error =>console.log(error));
  }

  back() {
    this.router.navigate(["project", this.project]);
  }
}
