import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { OtherAccountComponent } from './other-account.component';

describe('OtherAccountComponent', () => {
  let component: OtherAccountComponent;
  let fixture: ComponentFixture<OtherAccountComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ OtherAccountComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(OtherAccountComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
