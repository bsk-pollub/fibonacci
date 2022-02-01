import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FibcalcComponent } from './fibcalc.component';

describe('FibcalcComponent', () => {
  let component: FibcalcComponent;
  let fixture: ComponentFixture<FibcalcComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ FibcalcComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(FibcalcComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
