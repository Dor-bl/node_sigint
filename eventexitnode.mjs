import process from 'node:process';

process.on('exit', (code) => {
  if (code === 0) {
    console.log('Process exited normally.');
  } else if (code === 1) {
    console.log('Terminating the process...');
    // Perform any cleanup or final tasks here before the process exits.
  }
});

// Handle the 'SIGINT' event
process.on('SIGINT', () => {
  if (process.exitCode === undefined) {
    process.exitCode = 1;
    console.log('Received SIGINT signal. Press Ctrl+C again to terminate the process.');
  } else {
    // The process has already received SIGINT once; terminate it immediately.
    process.exit();
  }
});

// Optional: You can also display a message to inform the user about the behavior.
console.log('Press Ctrl+C to terminate the process.');

// Keep the Node.js event loop active to wait for the 'SIGINT' event.
process.stdin.resume();
