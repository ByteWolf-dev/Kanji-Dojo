import { Component, inject, OnInit, signal } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MatToolbarModule } from '@angular/material/toolbar';

import { AuthService } from '../../services/auth';
import { DojoService } from '../../services/dojo';
import { ProgressDto } from '../../dtos/progress-dto';

@Component({
    selector: 'app-dashboard',
    imports: [RouterLink, MatButtonModule, MatCardModule, MatProgressBarModule, MatToolbarModule],
    templateUrl: './dashboard.html',
    styleUrl: './dashboard.css'
})
export class Dashboard implements OnInit {

    private dojoService = inject(DojoService);
    private authService = inject(AuthService);
    private router = inject(Router);

    progress = signal<ProgressDto | null>(null);

    ngOnInit(): void {
        this.dojoService.getProgress().subscribe({
            next: (progress) => this.progress.set(progress),
            error: (err) => console.error('Failed to load progress', err)
        });
    }

    scorePercent(progress: ProgressDto): number {
        if (!progress.nextBeltRequiredScore) return 100;

        return Math.min(100, progress.score / progress.nextBeltRequiredScore * 100);
    }

    logout(): void {
        this.authService.logout().subscribe(() => this.router.navigate(['/login']));
    }
}
