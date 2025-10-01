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
\`---._.-------------------------------------------------------------._.---'`,
        keyMap: {
          "1": { index: 52, c: 1 },
          "2": { index: 54, c: 1 },
          "3": { index: 56, c: 1 },
          "4": { index: 58, c: 1 },
          "5": { index: 60, c: 1 },
          "6": { index: 62, c: 1 },
          "7": { index: 64, c: 1 },
          "8": { index: 66, c: 1 },
          "9": { index: 68, c: 1 },
          "0": { index: 70, c: 1 },
          "ß": { index: 72, c: 1 },
          "´": { index: 74, c: 1 },

          q: { index: 108, c: 1 },
          w: { index: 110, c: 1 },
          e: { index: 112, c: 1 },
          r: { index: 114, c: 1 },
          t: { index: 116, c: 1 },
          z: { index: 118, c: 1 },
          u: { index: 120, c: 1 },
          i: { index: 122, c: 1 },
          o: { index: 124, c: 1 },
          p: { index: 126, c: 1 },
          ü: { index: 128, c: 1 },
          "+": { index: 130, c: 1 },

          a: { index: 166, c: 1 },
          s: { index: 168, c: 1 },
          d: { index: 170, c: 1 },
          f: { index: 172, c: 1 },
          g: { index: 174, c: 1 },
          h: { index: 176, c: 1 },
          j: { index: 178, c: 1 },
          k: { index: 180, c: 1 },
          l: { index: 182, c: 1 },
          ö: { index: 184, c: 1 },
          ä: { index: 186, c: 1 },
          "#": { index: 188, c: 1 },

          "<": { index: 232, c: 1 },
          y: { index: 234, c: 1 },
          x: { index: 236, c: 1 },
          c: { index: 238, c: 1 },
          v: { index: 240, c: 1 },
          b: { index: 242, c: 1 },
          n: { index: 244, c: 1 },
          m: { index: 246, c: 1 },
          ",": { index: 248, c: 1 },
          ".": { index: 250, c: 1 },
          "-": { index: 252, c: 1 },

          "backspace": { index: 86, c: 3 },
          "shift": { index: 226, c: 5 },
          "control": { index: 296, c: 3 },
          " ": { index: 304, c: 25 },
        },
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
        this.highlightKey(e.key);
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
        if (key == "Enter") {
          const query = this.inputBuffer
            .substring(13, 87)
            .trim()
            .replaceAll('_', ' ')
          const uriQuery = encodeURIComponent(query)
          window.open(`https://www.google.com/search?q=${uriQuery}`, '_self')
        }
      },
      highlightKey(key) {
        const keyInfo = this.keyMap[key.toLowerCase()];
        if (!keyInfo) return;
        this.asciiKeyboard = this.asciiKeyboard.substring(0, keyInfo.index) + '_'.repeat(keyInfo.c) + this.asciiKeyboard.substring(keyInfo.index + keyInfo.c)
        setTimeout(() => {
          this.asciiKeyboard = this.asciiKeyboard.substring(0, keyInfo.index) + '-'.repeat(keyInfo.c) + this.asciiKeyboard.substring(keyInfo.index + keyInfo.c)
        }, 150);
      },
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