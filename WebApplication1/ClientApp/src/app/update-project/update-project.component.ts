import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { repeat } from 'rxjs/operators';
import { AuthorizeService } from 'src/api-authorization/authorize.service';
import { FetcherService } from '../fetcher.service';
import { Project } from '../models/project';

@Component({
  selector: 'app-update-project',
  templateUrl: './update-project.component.html',
  styleUrls: ['./update-project.component.css']
})
export class UpdateProjectComponent implements OnInit {
  public name: string = "";
  public users: string = "";
  private Id: string = "";
  public company: string = "";
  
  constructor(private fetcher: FetcherService, private authorizeService: AuthorizeService, private router: Router, private route: ActivatedRoute) { 
  }

  ngOnInit() {
    this.authorizeService.isAuthenticated().subscribe(auth => {
      if (!auth) {
        this.router.navigate(["/authentication/login"]);
      } else {
        this.Id = this.route.snapshot.paramMap.get('id');
        this.fetcher.getProjectFromApi(this.Id).subscribe(resp => {
          this.name = resp.name;
          this.company = resp.company;
          resp.assignees.forEach(a =>{
            this.fetcher.getAccountFromApiById(a).subscribe(resp => {
              if (resp.username !== undefined && !this.users.includes(resp.username))
              this.users += resp.username + ";"
            });
          });
        });
      }
    });
  }

  edit() {
    var p = new Project();
    p.id = this.Id;
    p.users = this.users;
    p.name = this.name;
    p.company = this.company;
    this.fetcher.postUpdateProjectToApi(p).subscribe(resp => this.router.navigate(["project", this.Id]), error =>console.log(error));
  }
}
