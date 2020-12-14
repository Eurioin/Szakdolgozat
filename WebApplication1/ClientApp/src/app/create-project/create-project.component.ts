import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthorizeService } from 'src/api-authorization/authorize.service';
import { FetcherService } from '../fetcher.service';
import { Project } from '../models/project';

@Component({
  selector: 'app-create-project',
  templateUrl: './create-project.component.html',
  styleUrls: ['./create-project.component.css']
})
export class CreateProjectComponent implements OnInit {
  public name: string = "";
  public users: string = "";
  public company: string = "";

  constructor(private fetcher: FetcherService, private authorizeService: AuthorizeService, private router: Router) { 
  }

  ngOnInit() {
    this.authorizeService.isAuthenticated().subscribe(auth => {
      if (!auth) {
        this.router.navigate(["/authentication/login"]);
      } else {
        var usr = sessionStorage.getItem('username');
        if (usr !== undefined && !this.users.includes(usr)) {
          this.users += usr + ';';
        }
      }
    });
  }

  create() {
    var p = new Project();
    p.users = [];
    p.users = p.users.concat(this.users.split(';'));
    p.name = this.name;
    p.company = this.company;
    this.fetcher.postNewProjectToApi(p).subscribe(resp => this.router.navigate(["projects"]), error =>console.log(error));
  }
}
