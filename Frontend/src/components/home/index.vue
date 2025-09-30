<template>
  <div class="container">
    <pre class="ascii">{{ asciiDesktopTop }}<br />{{ inputBuffer }}<br />{{ asciiDesktopBottom }}</pre>
    <pre class="ascii">{{ asciiKeyboard }}</pre>
  </div>
</template>

<script>
  export default {
    name: 'AsciiArt',
    data() {
      return {
        inputBuffer: '|   |   C:\\>                                                                           |    |',
        asciiDesktopTop: `__________________________________________________________________________________________
/                                                                                          \\
|    __________________________________________________________________________________     |
|   |                                                                                  |    |
|   |                                                                                  |    |`,

        asciiDesktopBottom: `|   |                                                                                  |    |
|   |                                                                                  |    |
|   |                                                                                  |    |
|   |                                                                                  |    |
|   |                                                                                  |    |
|   |                                                                                  |    |
|   |                                                                                  |    |
|   |                                                                                  |    |
|   |                                                                                  |    |
|   |                                                                                  |    |
|   |                                                                                  |    |
|   |                                                                                  |    |
|   |                                                                                  |    |
|   |                                                                                  |    |
|   |                                                                                  |    |
|   |__________________________________________________________________________________|    |
|                                                                                           |
\\__________________________________________________________________________________________/
\\____________________________________________________________________________/`,

        asciiKeyboard: `___________________________________________
_-'    .-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.  --- \`-_
_-'.-.-. .---.-.-.-.-.-.-.-.-.-.-.-.-.-.-.--.  .-.-.\`-_
_-'.-.-.-. .---.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-\`__\`. .-.-.-.\`-_
_-'.-.-.-.-. .-----.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-----. .-.-.-.-.\`-_
_-'.-.-.-.-.-. .---.-. .-------------------------. .-.---. .---.-.-.-.\`-_
: -------------------------------------------------------------------------:
\`---._.-------------------------------------------------------------._.---'`
      };
    },
    mounted() {
      window.addEventListener('keydown', this.handleKeydown);
    },
    beforeDestroy() {
      window.removeEventListener('keydown', this.handleKeydown);
    },
    methods: {
      handleKeydown(e) {
        // 85 spaces
        var key = e.key
        const count = (this.inputBuffer.match(/ /g) || []).length
        const diff = 85 - count
        if (key === 'Backspace') {
          this.inputBuffer = this.inputBuffer.substring(0, 12 + diff) + ' ' + this.inputBuffer.substring(13 + diff)
        }
        if (key.length === 1 || key.includes('^')) {
          if (key == ' ') key = '_'
          this.inputBuffer = this.inputBuffer.substring(0, 13 + diff) + key + this.inputBuffer.substring(13 + key.length + diff)
        }
        if (e.ctrlKey && key.toLowerCase() == 'c') {
          this.inputBuffer = '|   |   C:\\>                                                                           |    |'
        }
      }
    },
  }
</script>

<style scoped>
  .container {
    width: 100%;
    height: 98vh;
    align-content: center;
    font-family: ui-monospace, SFMono-Regular, Menlo, Monaco, 'Roboto Mono', 'Courier New', monospace;
  }

  .ascii {
    color: lime;
    font-size: 150%;
    line-height: 120%;
    margin: 0;
    text-align: center;
  }
</style>