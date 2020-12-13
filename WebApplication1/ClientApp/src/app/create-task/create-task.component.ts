import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthorizeService } from 'src/api-authorization/authorize.service';
import { FetcherService } from '../fetcher.service';
import { Project } from '../models/project';
import { SubTask } from '../models/sub-task';
import { Task } from '../models/task';

@Component({
  selector: 'app-create-task',
  templateUrl: './create-task.component.html',
  styleUrls: ['./create-task.component.css']
})
export class CreateTaskComponent implements OnInit {

  public Project: Project = new Project();
  public Users: Array<string> = [];
  public Selected: Array<boolean> = [];
  public name: string = "";
  public subtasks: string = "";
  public description: string = "";
  public type: string = "";
  public priority: number = 1;
  public date: Date = new Date();
  public project: string = "";

  constructor(private fetcher: FetcherService, private authorizeService: AuthorizeService, private router: Router, private route: ActivatedRoute) { 
  }

  ngOnInit() {
    this.authorizeService.isAuthenticated().subscribe(auth => {
      if (!auth) {
        this.router.navigate(["/authentication/login"]);
      } else {
        var usr = sessionStorage.getItem('username');
        if (usr !== undefined) {
          this.project = this.route.snapshot.paramMap.get('id');
          this.fetcher.getProjectFromApi(this.project).subscribe(resp => {
            this.Project = resp;
            this.Selected = [];
            this.Project.assignees.forEach(a => {
              this.fetcher.getAccountFromApiById(a).subscribe(r => {
                if (!this.Users.includes(r.username)) {
                  this.Users.push(r.username);
                }
              }, err => console.log(err));
              this.Selected.push(false);
            });
          }, err => console.log(err));
        }
      }
    });
  }

  create() {
    var t = new Task();
    t.name = this.name;
    t.description = this.description;
    t.endDate = this.date;
    t.priority = this.priority;
    t.type =this.type;
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
    this.fetcher.postNewTaskToApi(t).subscribe(resp => this.router.navigate(["project",this.project]), error =>console.log(error));
  }
}
