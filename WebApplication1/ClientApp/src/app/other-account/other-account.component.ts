import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthorizeService } from 'src/api-authorization/authorize.service';
import { FetcherService } from '../fetcher.service';
import { MongoAccount } from '../models/mongo-account';

@Component({
  selector: 'app-other-account',
  templateUrl: './other-account.component.html',
  styleUrls: ['./other-account.component.css']
})
export class OtherAccountComponent implements OnInit {

  public Account: MongoAccount = new MongoAccount();
  public CurrentUser: MongoAccount = new MongoAccount();
  public name: string;
  public phoneNumber: string;
  public email: string;
  public roles: string = "";
  
  constructor(private fetcher: FetcherService, private authorizeService: AuthorizeService, private router: Router, private route: ActivatedRoute) { 
  }

  ngOnInit() {
    var i = 0;
    this.authorizeService.isAuthenticated().subscribe(auth => {
      if (i++ === 0) {
        if (!auth) {
          this.router.navigate(["/authentication/login"]);
        } else {
          this.fetcher.getAccountFromApi(sessionStorage.getItem('username')).subscribe(resp => {
            this.CurrentUser = resp;
            this.fetcher.getAccountFromApiById(this.route.snapshot.paramMap.get('id')).subscribe(respb => {
              if (this.Account.username === undefined) {
                this.Account = respb;
                this.email = this.Account.email;
                this.phoneNumber = this.Account.phoneNumber;
                this.name = this.Account.name;
                if (this.CurrentUser.uniqueRoles.includes('ADMIN')) {
                  respb.uniqueRoles.forEach( r => {
                    if (r !== undefined && !this.roles.includes(r)) {
                      this.roles += r + ';';
                    }
                  });
                } else {
                  this.roles = "";
                  this.Account.uniqueRoles = [];
                }
              }
            }, error => console.log(error));
          }, err => console.log(err));
        }
      }
    });
  }

  edit() {
    this.Account.phoneNumber = this.phoneNumber;
    this.Account.email = this.email;
    this.Account.name = this.name;
    this.Account.roles = this.roles;
    this.fetcher.postUpdateAccountToApi(this.Account).subscribe(resp => this.Account = resp, err => console.log(err))
  }
}
