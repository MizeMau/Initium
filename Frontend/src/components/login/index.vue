<template>
  <div class="login-page">
    <!-- Starfield background -->
    <canvas id="stars-canvas" ref="canvas"></canvas>

    <div class="login-card">
      <h2 class="title">Login</h2>

      <div class="form-group">
        <label for="username">Username</label>
        <input id="username"
               v-model="userName"
               type="text"
               placeholder="Enter username"
               required />
      </div>

      <div class="form-group">
        <label for="password">Password</label>
        <input id="password"
               v-model="password"
               type="password"
               placeholder="Enter password"
               required />
      </div>

      <button @click="login" :disabled="isLoading" class="btn">
        <span v-if="!isLoading">Login</span>
        <span v-else>Logging in...</span>
      </button>

      <p v-if="error" class="error">{{ error }}</p>
    </div>
  </div>
</template>

<script>
  import { useUserStore } from '@/stores/authStore'

  export default {
    name: 'LoginPage',
    data() {
      return {
        userName: '',
        password: '',
        isLoading: false,
        error: null,
        userStore: useUserStore(),
        animationFrame: null,
        stars: [],
        FPS: 60,
        numStars: 120,
        mouse: { x: 0, y: 0 },
        ctx: null,
      }
    },
    mounted() {
      const canvas = this.$refs.canvas
      this.ctx = canvas.getContext('2d')

      const resizeCanvas = () => {
        canvas.width = window.innerWidth
        canvas.height = window.innerHeight
      }
      resizeCanvas()
      window.addEventListener('resize', resizeCanvas)

      // Generate stars with random color
      const palette = ['#fbc531']
      for (let i = 0; i < this.numStars; i++) {
        this.stars.push({
          x: Math.random() * canvas.width,
          y: Math.random() * canvas.height,
          radius: Math.random() * 1.5 + 0.5,
          vx: Math.floor(Math.random() * 50) - 25,
          vy: Math.floor(Math.random() * 50) - 25,
          color: palette[Math.floor(Math.random() * palette.length)],
        })
      }

      window.addEventListener('mousemove', this.onMouseMove)
      this.tick()
    },
    beforeDestroy() {
      cancelAnimationFrame(this.animationFrame)
      window.removeEventListener('mousemove', this.onMouseMove)
    },
    methods: {
      async login() {
        this.isLoading = true
        this.error = null
        try {
          await this.userStore.login(this.userName, this.password)
          this.$router.push('/')
        } catch (err) {
          console.error(err)
          this.error = 'Invalid username or password.'
        } finally {
          this.isLoading = false
        }
      },
      distance(a, b) {
        const dx = a.x - b.x
        const dy = a.y - b.y
        return Math.sqrt(dx * dx + dy * dy)
      },
      draw() {
        const ctx = this.ctx
        const canvas = this.$refs.canvas
        ctx.clearRect(0, 0, canvas.width, canvas.height)
        ctx.globalCompositeOperation = 'lighter'

        // Draw stars
        for (const s of this.stars) {
          ctx.fillStyle = s.color
          ctx.beginPath()
          ctx.arc(s.x, s.y, s.radius, 0, 2 * Math.PI)
          ctx.fill()
        }

        // Draw connections
        ctx.beginPath()
        for (const starI of this.stars) {
          ctx.moveTo(starI.x, starI.y)
          if (this.distance(this.mouse, starI) < 150)
            ctx.lineTo(this.mouse.x, this.mouse.y)
          for (const starII of this.stars) {
            if (this.distance(starI, starII) < 150)
              ctx.lineTo(starII.x, starII.y)
          }
        }
        ctx.lineWidth = 0.05
        ctx.strokeStyle = '#fb9931'
        //ctx.strokeStyle = 'white'
        ctx.stroke()
      },
      update() {
        const canvas = this.$refs.canvas
        for (const s of this.stars) {
          s.x += s.vx / this.FPS
          s.y += s.vy / this.FPS
          if (s.x < 0 || s.x > canvas.width) s.vx *= -1
          if (s.y < 0 || s.y > canvas.height) s.vy *= -1
        }
      },
      tick() {
        this.draw()
        this.update()
        this.animationFrame = requestAnimationFrame(this.tick)
      },
      onMouseMove(e) {
        this.mouse.x = e.clientX
        this.mouse.y = e.clientY
      },
    },
  }
</script>

<style scoped>
  .login-page {
    position: relative;
    display: flex;
    justify-content: center;
    align-items: center;
    min-height: 100vh;
    overflow: hidden;
  }

  #stars-canvas {
    position: fixed;
    top: 0;
    left: 0;
    z-index: 0;
    width: 100%;
    height: 100%;
    background: radial-gradient(circle at bottom, #0d0d0d, #000);
  }

  /* Card */
  .login-card {
    position: relative;
    z-index: 2;
    background-color: rgba(30, 30, 30, 0.9);
    padding: 2.5rem 2rem;
    border-radius: 16px;
    box-shadow: 0 6px 16px rgba(0, 0, 0, 0.6);
    width: 100%;
    max-width: 360px;
    text-align: center;
    animation: fadeIn 0.4s ease-in-out;
  }

  .title {
    margin-top: 0rem;
    margin-bottom: 2rem;
    color: #fbc531;
    font-size: 1.8rem;
    font-weight: 600;
  }

  .form-group {
    text-align: left;
    margin-bottom: 1.2rem;
  }

  .form-group {
    text-align: left;
    margin-bottom: 1.2rem;
  }

    /* Make labels lighter for dark theme */
    .form-group label {
      display: block;
      font-size: 0.9rem;
      font-weight: 500;
      color: #aaa;
      margin-bottom: 0.4rem;
    }

    /* FIXED: perfectly align inputs with button */
    .form-group input {
      display: block;
      width: 100%;
      box-sizing: border-box; /* ensures padding doesn’t affect total width */
      padding: 0.75rem 0.8rem;
      border: 1px solid #333;
      border-radius: 8px;
      font-size: 1rem;
      color: #fff;
      background-color: #121212;
      outline: none;
      transition: border-color 0.2s, box-shadow 0.2s;
    }

      /* Focus effect */
      .form-group input:focus {
        border-color: #4a6cf7;
        box-shadow: 0 0 0 3px rgba(74, 108, 247, 0.25);
      }


  .btn {
    width: 100%;
    margin-top: 2rem;
    padding: 0.75rem;
    font-size: 1rem;
    background-color: #4a6cf7;
    color: #fff;
    border: none;
    border-radius: 8px;
    cursor: pointer;
    font-weight: 600;
    transition: background-color 0.2s, transform 0.1s;
  }

    .btn:hover {
      background-color: #3957d0;
    }

    .btn:active {
      transform: scale(0.98);
    }

    .btn:disabled {
      background-color: #9ab1fa;
      cursor: not-allowed;
    }

  .error {
    margin-top: 0.5rem;
    color: #e74c3c;
    font-size: 0.9rem;
  }

  @keyframes fadeIn {
    from {
      opacity: 0;
      transform: translateY(15px);
    }

    to {
      opacity: 1;
      transform: translateY(0);
    }
  }
</style>
